import { Component, Injector, ViewChild } from '@angular/core';
import { TokenService } from '@eaf/auth/token.service';
import { IAjaxResponse } from '@eaf/eafHttpInterceptor';
import { AppConsts } from '@shared/AppConsts';
import { AppComponentBase } from '@shared/common/app-component-base';
import { ProfileServiceProxy, UpdateProfilePictureInput } from '@shared/service-proxies/service-proxies';
import { FileUploader, FileUploaderOptions } from 'ng2-file-upload';
import { ModalDirective } from 'ngx-bootstrap';
import { ImageCroppedEvent } from 'ngx-image-cropper';
import { finalize } from 'rxjs/operators';

@Component({
  selector: 'changeProfilePictureModal',
  templateUrl: './change-profile-picture-modal.component.html',
})
export class ChangeProfilePictureModalComponent extends AppComponentBase {
  @ViewChild('changeProfilePictureModal', { static: true }) modal: ModalDirective;

  public active = false;
  public uploader: FileUploader;
  public temporaryPictureUrl: string;
  public saving = false;
  private input = new UpdateProfilePictureInput();

  public maxProfilPictureBytesUserFriendlyValue = 50;
  private temporaryPictureFileName: string;
  private _uploaderOptions: FileUploaderOptions = {};

  imageChangedEvent: any = '';

  constructor(
    injector: Injector,
    private _profileService: ProfileServiceProxy,
    private _tokenService: TokenService,
  ) {
    super(injector);
  }

  initializeModal(): void {
    this.active = true;
    this.temporaryPictureUrl = '';
    this.temporaryPictureFileName = '';
    this.initFileUploader();
  }

  show(): void {
    this.initializeModal();
    this.modal.show();
  }

  close(): void {
    this.active = false;
    this.imageChangedEvent = '';
    this.uploader.clearQueue();
    this.modal.hide();
  }

  fileChangeEvent(event: any): void {
    if (event.target.files[0].size > 5242880) {
      //5MB
      this.message.warn(this.l('ProfilePicture_Warn_SizeLimit', this.maxProfilPictureBytesUserFriendlyValue));
      return;
    }

    this.imageChangedEvent = event;
    this.uploader.clearQueue();
    this.uploader.addToQueue(event.target.files);
  }

  imageCropped(event: ImageCroppedEvent): void {
    this.input.x = event.cropperPosition.x1;
    this.input.y = event.cropperPosition.y1;
    this.input.width = event.cropperPosition.x2;
    this.input.height = event.cropperPosition.y2;
  }

  imageCroppedFile(file: File): void {
    const files: File[] = [file];
    this.uploader.clearQueue();
    this.uploader.addToQueue(files);
  }

  initFileUploader(): void {
    this.input = new UpdateProfilePictureInput();
    this.uploader = new FileUploader({ url: AppConsts.remoteServiceBaseUrl + '/api/services/app/Profile/UploadProfilePicture' });
    this._uploaderOptions.autoUpload = false;
    this._uploaderOptions.authToken = 'Bearer ' + this._tokenService.getToken();
    this._uploaderOptions.removeAfterUpload = true;
    this.uploader.onAfterAddingFile = file => {
      file.withCredentials = false;
    };

    this.uploader.onSuccessItem = (item, response, status) => {
      const resp = <IAjaxResponse>JSON.parse(response);
      if (resp.success) {
        this.updateProfilePicture(resp.result.fileToken);
      } else {
        this.message.error(resp.error.message);
      }
    };

    this.uploader.setOptions(this._uploaderOptions);
  }

  updateProfilePicture(fileToken: string): void {
    console.warn(this.imageChangedEvent);
    this.input.fileToken = fileToken;

    this.saving = true;
    this._profileService
      .updateProfilePicture(this.input)
      .pipe(
        finalize(() => {
          this.saving = false;
        }),
      )
      .subscribe(() => {
        eaf.event.trigger('profilePictureChanged');
        this.close();
      });
  }

  guid(): string {
    function s4() {
      return Math.floor((1 + Math.random()) * 0x10000)
        .toString(16)
        .substring(1);
    }
    return s4() + s4() + '-' + s4() + '-' + s4() + '-' + s4() + '-' + s4() + s4() + s4();
  }

  save(): void {
    console.info(this.uploader.queue);
    this.uploader.uploadAll();
  }
}
