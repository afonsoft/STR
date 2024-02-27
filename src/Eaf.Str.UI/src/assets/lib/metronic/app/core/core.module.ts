import { CommonModule } from '@angular/common';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { EafHttpInterceptor } from '@eaf/eafHttpInterceptor';

import { MenuAsideOffcanvasDirective } from './directives/menu-aside-offcanvas.directive';
import { MenuAsideToggleDirective } from './directives/menu-aside-toggle.directive';
import { MenuAsideDirective } from './directives/menu-aside.directive';
import { MenuHorizontalOffcanvasDirective } from './directives/menu-horizontal-offcanvas.directive';
import { MenuHorizontalDirective } from './directives/menu-horizontal.directive';
import { LayoutRefService } from './services/layout-ref.service';

@NgModule({
  imports: [CommonModule],
  declarations: [
    // directives
    MenuAsideDirective,
    MenuAsideOffcanvasDirective,
    MenuHorizontalOffcanvasDirective,
    MenuHorizontalDirective,
    MenuAsideToggleDirective,
  ],
  exports: [
    // directives
    MenuAsideDirective,
    MenuAsideOffcanvasDirective,
    MenuHorizontalOffcanvasDirective,
    MenuHorizontalDirective,
    MenuAsideToggleDirective,
  ],
  providers: [
    LayoutRefService,
    {
      provide: HTTP_INTERCEPTORS,
      useClass: EafHttpInterceptor,
      multi: true,
    },
  ],
})
export class CoreModule {}
