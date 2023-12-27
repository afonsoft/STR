import { Component, EventEmitter, Injector, Input, Output, ViewEncapsulation } from '@angular/core';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { AppComponentBase } from '@shared/common/app-component-base';
import { AwbAddressDto, ViaCepServiceProxy } from '@shared/service-proxies/service-proxies';

@Component({
  selector: 'app-address',
  templateUrl: './address.component.html',
  encapsulation: ViewEncapsulation.None,
  animations: [appModuleAnimation()],
})
export class AddressComponent extends AppComponentBase {
  @Input() enabled = false;
  @Input() name: string = '';
  @Input() public address: AwbAddressDto = <AwbAddressDto>{};
  @Output() updateAddress: EventEmitter<AwbAddressDto> = new EventEmitter<AwbAddressDto>();

  mask: Array<string | RegExp>;

  constructor(
    injector: Injector,
    private _cepService: ViaCepServiceProxy,
  ) {
    super(injector);
    this.mask = [/\d/, /\d/, /\d/, /\d/, /\d/, '-', /\d/, /\d/, /\d/];
  }

  private findCep(cep: string): void {
    this.address = new AwbAddressDto();
    this.dataTableHelper.showLoadingIndicator();
    this._cepService.getAddress(cep).subscribe(result => {
      this.address.neighborhood = result.bairro;
      this.address.state = result.uf;
      this.address.zipCode = result.cep;
      this.address.street = result.logradouro;
      this.address.complement = result.complemento;
      this.address.city = result.localidade;
      this.onChange();
      this.dataTableHelper.hideLoadingIndicator();
    });
  }

  onChangeCep(event?: any) {
    this.findCep(event.target.value);
  }

  onChange(event?: any) {
    this.updateAddress.emit(this.address);
  }
}
