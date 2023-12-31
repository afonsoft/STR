import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { AppCommonModule } from '@app/shared/common/app-common.module';
import { UtilsModule } from '@shared/utils/utils.module';
import { CountoModule } from 'angular2-counto';
import { ModalModule, TabsModule, TooltipModule, BsDropdownModule, PopoverModule } from 'ngx-bootstrap';
import { DashboardComponent } from './dashboard/dashboard.component';
import { MainRoutingModule } from './main-routing.module';
import { NgxChartsModule } from '@swimlane/ngx-charts';

import { BsDatepickerModule, BsDatepickerConfig, BsDaterangepickerConfig, BsLocaleService } from 'ngx-bootstrap/datepicker';
import { NgxBootstrapDatePickerConfigService } from 'assets/lib/ngx-bootstrap/ngx-bootstrap-datepicker-config.service';
import { AirplanesComponent } from './airplanes/airplanes.component';
import { CreateOrEditAirplaneModalComponent } from './airplanes/create-or-edit-airplane-modal.component';
import { FileUploadModule } from 'ng2-file-upload';

import { InputMaskModule } from 'primeng/inputmask';
import { FileUploadModule as PrimeNgFileUploadModule } from 'primeng/fileupload';
import { TreeModule } from 'primeng/tree';
import { DragDropModule } from 'primeng/dragdrop';
import { PaginatorModule } from 'primeng/paginator';
import { ContextMenuModule } from 'primeng/contextmenu';
import { AutoCompleteModule } from 'primeng/autocomplete';
import { EditorModule } from 'primeng/editor';

import { TableModule } from 'primeng/table';
import { TextMaskModule } from 'angular2-text-mask';
import { ImageCropperModule } from 'ngx-image-cropper';
import { TimepickerModule } from 'ngx-bootstrap/timepicker';
import { NgxMaskModule } from 'ngx-mask';
import { CalendarModule, DateAdapter } from 'angular-calendar';
import { adapterFactory } from 'angular-calendar/date-adapters/date-fns';
import { AngularEditorModule } from '@kolkov/angular-editor';
import { NgMultiSelectDropDownModule } from 'ng-multiselect-dropdown';
import { AwbComponent } from './awb/awb.component';
import { CepComponent } from './cep/cep.component';
import { TrackingComponent } from './tracking/tracking.component';
import { AirportsComponent } from './airports/airports.component';
import { CreateOrEditAwbComponent } from './awb/create-or-edit-awb/create-or-edit-awb.component';

NgxBootstrapDatePickerConfigService.registerNgxBootstrapDatePickerLocales();

@NgModule({
  imports: [
    FormsModule,
    ReactiveFormsModule,
    CommonModule,
    FileUploadModule,
    PrimeNgFileUploadModule,
    ModalModule.forRoot(),
    TabsModule.forRoot(),
    TooltipModule.forRoot(),
    PopoverModule.forRoot(),
    BsDropdownModule.forRoot(),
    BsDatepickerModule.forRoot(),
    TimepickerModule.forRoot(),
    NgMultiSelectDropDownModule.forRoot(),
    NgxMaskModule.forRoot(),
    MainRoutingModule,
    UtilsModule,
    AppCommonModule,
    TableModule,
    TreeModule,
    DragDropModule,
    ContextMenuModule,
    PaginatorModule,
    AutoCompleteModule,
    EditorModule,
    InputMaskModule,
    NgxChartsModule,
    CountoModule,
    TextMaskModule,
    ImageCropperModule,
    AngularEditorModule,
    CalendarModule.forRoot({
      provide: DateAdapter,
      useFactory: adapterFactory,
    }),
  ],
  declarations: [DashboardComponent, AirplanesComponent, CreateOrEditAirplaneModalComponent, AwbComponent, CepComponent, TrackingComponent, AirportsComponent, CreateOrEditAwbComponent],
  providers: [
    { provide: BsDatepickerConfig, useFactory: NgxBootstrapDatePickerConfigService.getDatepickerConfig },
    { provide: BsDaterangepickerConfig, useFactory: NgxBootstrapDatePickerConfigService.getDaterangepickerConfig },
    { provide: BsLocaleService, useFactory: NgxBootstrapDatePickerConfigService.getDatepickerLocale },
  ],
})
export class MainModule {}
