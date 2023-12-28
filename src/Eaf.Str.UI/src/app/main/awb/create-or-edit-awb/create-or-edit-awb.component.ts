import { Component, EventEmitter, Injector, OnInit, Output, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
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
export class CreateOrEditAwbComponent extends AppComponentBase implements OnInit {
  @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
  @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

  @ViewChild('dataTable', { static: true }) dataTable: Table;

  active = false;
  saving = false;
  isCreate = false;

  awbCreate: CreateOrEditAwbDto = new CreateOrEditAwbDto();

  public AwbForm: FormGroup;
  public formAddressSenderGroup: FormGroup;
  public formAddressRecipientGroup: FormGroup;

  constructor(
    injector: Injector,
    private _awbServiceProxy: AwbServiceProxy,
    private fb: FormBuilder,
  ) {
    super(injector);
    this.dataTableHelper.defaultRecordsCountPerPage = 9999;
  }

  ngOnInit() {
    this.awbCreate = new CreateOrEditAwbDto();
    this.awbCreate.sender = new AwbAddressDto();
    this.awbCreate.recipient = new AwbAddressDto();

    this.createForm();
  }
  createForm() {
    this.AwbForm = this.fb.group({
      trackingNumber: [this.awbCreate.trackingNumber],
      recipient: [this.awbCreate.recipient, [Validators.required]],
      sender: [this.awbCreate.sender, [Validators.required]],
      origin: [this.awbCreate.origin, [Validators.required, Validators.maxLength(3), Validators.minLength(3)]],
      destiny: [this.awbCreate.destiny, [Validators.required, Validators.maxLength(3), Validators.minLength(3)]],
      receivedName: [this.awbCreate.receivedName],
      receivedDocument: [this.awbCreate.receivedDocument],
      itens: [this.awbCreate.itens, [Validators.required]],
      id: [this.awbCreate.id],
    });

    this.formAddressSenderGroup = this.fb.group({
      zipCode: [this.awbCreate.sender.zipCode, [Validators.required]],
      personName: [this.awbCreate.sender.personName, [Validators.maxLength(128)]],
      observation: [this.awbCreate.sender.observation, [Validators.maxLength(128)]],
      street: [this.awbCreate.sender.street, [Validators.required, Validators.maxLength(256), Validators.minLength(5)]],
      neighborhood: [this.awbCreate.sender.neighborhood, [Validators.required, Validators.maxLength(256), Validators.minLength(5)]],
      city: [this.awbCreate.sender.city, [Validators.required, Validators.maxLength(256), Validators.minLength(5)]],
      state: [this.awbCreate.sender.state, [Validators.required, Validators.maxLength(256), Validators.minLength(5)]],
      complement: [this.awbCreate.sender.complement, [Validators.maxLength(128)]],
    });

    this.formAddressRecipientGroup = this.fb.group({
      zipCode: [this.awbCreate.recipient.zipCode, [Validators.required]],
      personName: [this.awbCreate.recipient.personName, [Validators.maxLength(128)]],
      observation: [this.awbCreate.recipient.observation, [Validators.maxLength(128)]],
      street: [this.awbCreate.recipient.street, [Validators.required, Validators.maxLength(256), Validators.minLength(5)]],
      neighborhood: [this.awbCreate.recipient.neighborhood, [Validators.required, Validators.maxLength(256), Validators.minLength(5)]],
      city: [this.awbCreate.recipient.city, [Validators.required, Validators.maxLength(256), Validators.minLength(5)]],
      state: [this.awbCreate.recipient.state, [Validators.required, Validators.maxLength(256), Validators.minLength(5)]],
      complement: [this.awbCreate.recipient.complement, [Validators.maxLength(128)]],
    });
  }

  show(Id?: number): void {
    this.awbCreate = new CreateOrEditAwbDto();
    this.awbCreate.sender = new AwbAddressDto();
    this.awbCreate.recipient = new AwbAddressDto();
    this.createForm();

    if (!Id) {
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

  checkIsValidForm() {
    if (!this.formAddressSenderGroup.valid || !this.formAddressRecipientGroup.valid || !this.AwbForm.valid) return false;
    return true;
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
