import { Component, EventEmitter, Injector, Output, ViewChild } from '@angular/core';
import { AddressComponent } from '@app/shared/common/address/address.component';
import { AppComponentBase } from '@shared/common/app-component-base';
import { AwbAddressDto, AwbServiceProxy, CreateOrEditAwbDto } from '@shared/service-proxies/service-proxies';
import { ModalDirective } from 'ngx-bootstrap';
import { LazyLoadEvent } from 'primeng/primeng';
import { Table } from 'primeng/table';
import { finalize } from 'rxjs/operators';

@Component({
  selector: 'createOrEditAwbModel',
  templateUrl: './create-or-edit-awb.component.html',
  styleUrls: ['./create-or-edit-awb.component.css'],
})
export class CreateOrEditAwbComponent extends AppComponentBase {
  @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
  @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

  @ViewChild('addressSender', { static: true }) addressSender: AddressComponent;
  @ViewChild('addressRecipient', { static: true }) addressRecipient: AddressComponent;
  @ViewChild('dataTable', { static: true }) dataTable: Table;

  active = false;
  saving = false;
  isCreate = false;

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
      this.isCreate = true;
      this.modal.show();
    } else {
      this._awbServiceProxy.getForEdit(Id).subscribe(result => {
        this.awbCreate = result;
        this.active = true;
        this.isCreate = false;
        this.modal.show();
      });
    }
  }

  save(): void {
    this.saving = true;
    console.log(this.awbCreate);
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

  getAwbItens(event?: LazyLoadEvent): void {}

  onChangeSender(address: AwbAddressDto) {
    console.log(address);
    this.awbCreate.sender = address;
  }
  onChangeRecipient(address: AwbAddressDto) {
    console.log(address);
    this.awbCreate.recipient = address;
  }
}
