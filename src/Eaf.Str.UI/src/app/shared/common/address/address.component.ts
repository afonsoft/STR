import { Component, EventEmitter, Injector, Input, OnInit, Output, ViewEncapsulation } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { AppComponentBase } from '@shared/common/app-component-base';
import { AwbAddressDto, ViaCepServiceProxy } from '@shared/service-proxies/service-proxies';

@Component({
  selector: 'app-address',
  templateUrl: './address.component.html',
  encapsulation: ViewEncapsulation.None,
  animations: [appModuleAnimation()],
})
export class AddressComponent extends AppComponentBase implements OnInit {
  @Input() enabled = false;
  @Input() name: string = '';
  @Input() public formAddressGroup: FormGroup;
  @Output() updateAddress: EventEmitter<AwbAddressDto> = new EventEmitter<AwbAddressDto>();

  mask: Array<string | RegExp>;

  constructor(
    injector: Injector,
    private _cepService: ViaCepServiceProxy,
  ) {
    super(injector);
  }

  ngOnInit() {
    this.mask = [/\d/, /\d/, /\d/, /\d/, /\d/, '-', /\d/, /\d/, /\d/];
  }

  private findCep(cep: string): void {
    this.dataTableHelper.showLoadingIndicator();
    this._cepService.getAddress(cep).subscribe(result => {
      this.formAddressGroup.controls.neighborhood.setValue(result.bairro);
      this.formAddressGroup.controls.state.setValue(result.uf);
      this.formAddressGroup.controls.zipCode.setValue(result.cep);
      this.formAddressGroup.controls.street.setValue(result.logradouro);
      this.formAddressGroup.controls.complement.setValue(result.complemento);
      this.formAddressGroup.controls.city.setValue(result.localidade);
      this.dataTableHelper.hideLoadingIndicator();
    });
  }

  onChangeCep(event?: any) {
    this.findCep(event.target.value);
  }

  onChange(event?: any) {
    const address = new AwbAddressDto();
    address.neighborhood = this.formAddressGroup.controls.neighborhood.value;
    address.zipCode = this.formAddressGroup.controls.zipCode.value;
    address.state = this.formAddressGroup.controls.state.value;
    address.street = this.formAddressGroup.controls.street.value;
    address.complement = this.formAddressGroup.controls.complement.value;
    address.city = this.formAddressGroup.controls.city.value;

    address.observation = this.formAddressGroup.controls.observation.value;
    address.personName = this.formAddressGroup.controls.personName.value;

    this.updateAddress.emit(address);
  }
}
