import { Component, EventEmitter, Injector, Output, ViewChild } from '@angular/core';
import { AppComponentBase } from '@shared/common/app-component-base';
import { AwbDto, AwbServiceProxy, CreateOrEditAwbDto } from '@shared/service-proxies/service-proxies';
import { ModalDirective } from 'ngx-bootstrap';
import { finalize } from 'rxjs/operators';

@Component({
  selector: 'createOrEditAwbModel',
  templateUrl: './create-or-edit-awb.component.html',
  styleUrls: ['./create-or-edit-awb.component.css'],
})
export class CreateOrEditAwbComponent extends AppComponentBase {
  @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
  @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

  active = false;
  saving = false;

  awb: AwbDto = new AwbDto();
  awbCreate: CreateOrEditAwbDto = new CreateOrEditAwbDto();

  constructor(
    injector: Injector,
    private _awbServiceProxy: AwbServiceProxy,
  ) {
    super(injector);
    this.dataTableHelper.defaultRecordsCountPerPage = 9999;
  }

  show(Id?: number): void {
    if (!Id) {
      this.awbCreate = new CreateOrEditAwbDto();
      this.active = true;
      this.modal.show();
    } else {
      this._awbServiceProxy.getForEdit(Id).subscribe(result => {
        this.awbCreate = result;
        this.active = true;
        this.modal.show();
      });
    }
  }

  onShown(): void {
    document.getElementById('Number').focus();
  }

  save(): void {
    this.saving = true;
    this._awbServiceProxy
      .createOrUpdate(this.awbCreate)
      .pipe(
        finalize(() => {
          this.saving = false;
        }),
      )
      .subscribe(() => {
        this.notify.info(this.l('SavedSuccessfully'));
        this.close();
        this.modalSave.emit(null);
      });
  }

  close(): void {
    this.active = false;
    this.modal.hide();
  }
}
