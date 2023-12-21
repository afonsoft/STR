import { Component, EventEmitter, Injector, Output, ViewChild } from '@angular/core';
import { AppComponentBase } from '@shared/common/app-component-base';
import { AwbDto, AwbServiceProxy, CreateOrEditAirplaneDto } from '@shared/service-proxies/service-proxies';
import { ModalDirective } from 'ngx-bootstrap';
import { finalize } from 'rxjs/operators';

@Component({
  selector: 'app-create-or-edit-awb',
  templateUrl: './create-or-edit-awb.component.html',
  styleUrls: ['./create-or-edit-awb.component.css']
})
export class CreateOrEditAwbComponent extends AppComponentBase {
  @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
  @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

  active = false;
  saving = false;

  airplane: AwbDto = new AwbDto();

  
  constructor(
    injector: Injector,
    private _awbServiceProxy: AwbServiceProxy,
  ) {
    super(injector);
  }
  ngOnInit(): void {
  }
  show(airplaneId?: number): void {
    if (!airplaneId) {
      this.airplane = new CreateOrEditAirplaneDto();
      this.active = true;
      this.modal.show();
    } else {
      this._airplanesServiceProxy.getAirplaneForEdit(airplaneId).subscribe(result => {
        this.airplane = result;
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
    this._airplanesServiceProxy
      .createOrEdit(this.airplane)
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

}