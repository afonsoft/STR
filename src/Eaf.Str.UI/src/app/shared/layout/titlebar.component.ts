import { Component, Injector } from '@angular/core';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
  selector: 'titlebar',
  template: `<h3 class="m_header_tilebar header-{{ currentTheme.baseSettings.header.headerSkin }}">
    Sistema de Transporte e Rastreamento
  </h3>`,
})
export class TitleBarComponent extends AppComponentBase {
  constructor(injector: Injector) {
    super(injector);
  }
}
