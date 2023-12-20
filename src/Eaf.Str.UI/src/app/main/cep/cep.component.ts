import { Component, Injector, OnInit, ViewEncapsulation } from '@angular/core';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { AppComponentBase } from '@shared/common/app-component-base';
import { ViaCepAddressDto, ViaCepServiceProxy } from '@shared/service-proxies/service-proxies';

@Component({
  selector: 'app-cep',
  templateUrl: './cep.component.html',
  styleUrls: ['./cep.component.css'],
  encapsulation: ViewEncapsulation.None,
  animations: [appModuleAnimation()],
})
export class CepComponent extends AppComponentBase implements OnInit {
  filters: {
    filterText: string;
  } = <any>{};

  public viaCepAddressDto: ViaCepAddressDto = <ViaCepAddressDto>{};
  public mask: Array<string | RegExp>;

  constructor(
    injector: Injector,
    private _viaCepServiceProxy: ViaCepServiceProxy,
  ) {
    super(injector);
    this.mask = [/\d/, /\d/, /\d/, /\d/, /\d/, '-', /\d/, /\d/, /\d/];
    this.viaCepAddressDto = new ViaCepAddressDto();
  }
  ngOnInit(): void {}

  getCep() {
    this.dataTableHelper.showLoadingIndicator();
    this.viaCepAddressDto = new ViaCepAddressDto();
    this._viaCepServiceProxy.getAddress(this.filters.filterText).subscribe(result => {
      this.viaCepAddressDto = result;
      this.dataTableHelper.hideLoadingIndicator();
    });
  }
}
