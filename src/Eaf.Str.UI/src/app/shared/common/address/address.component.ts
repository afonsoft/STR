import { Component, Injector, Input, OnInit, ViewEncapsulation } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { AppComponentBase } from '@shared/common/app-component-base';
import { ViaCepServiceProxy } from '@shared/service-proxies/service-proxies';

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

  mask: Array<string | RegExp>;

  constructor(
    injector: Injector,
    private _cepService: ViaCepServiceProxy,
    private fb: FormBuilder,
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
}
