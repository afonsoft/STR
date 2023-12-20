import { Component, Injector, OnInit, ViewChild, ViewEncapsulation } from '@angular/core';
import { EntityTypeHistoryModalComponent } from '@app/shared/common/entityHistory/entity-type-history-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { AppComponentBase } from '@shared/common/app-component-base';
import { AirplaneDto, AirplanesServiceProxy } from '@shared/service-proxies/service-proxies';
import { FileDownloadService } from '@shared/utils/file-download.service';
import * as _ from 'lodash';
import { Paginator } from 'primeng/paginator';
import { LazyLoadEvent } from 'primeng/primeng';
import { Table } from 'primeng/table';

import { CreateOrEditAirplaneModalComponent } from './create-or-edit-airplane-modal.component';

@Component({
  templateUrl: './airplanes.component.html',
  encapsulation: ViewEncapsulation.None,
  animations: [appModuleAnimation()],
})
export class AirplanesComponent extends AppComponentBase implements OnInit {
  @ViewChild('createOrEditAirplaneModal', { static: true }) createOrEditAirplaneModal: CreateOrEditAirplaneModalComponent;
  @ViewChild('entityTypeHistoryModal', { static: true }) entityTypeHistoryModal: EntityTypeHistoryModalComponent;
  @ViewChild('dataTable', { static: true }) dataTable: Table;
  @ViewChild('paginator', { static: true }) paginator: Paginator;

  filters: {
    filterText: string;
  } = <any>{};

  _entityTypeFullName = 'Eaf.Str.Airplanes.Airplane';
  entityHistoryEnabled = false;

  constructor(
    injector: Injector,
    private _airplanesServiceProxy: AirplanesServiceProxy,
    private _fileDownloadService: FileDownloadService,
  ) {
    super(injector);
  }

  ngOnInit(): void {
    this.registerEvents();
    const customSettings = (eaf as any).custom;
    this.entityHistoryEnabled =
      customSettings.EntityHistory &&
      customSettings.EntityHistory.isEnabled &&
      _.filter(customSettings.EntityHistory.enabledEntities, entityType => entityType === this._entityTypeFullName).length === 1;
  }

  getAirplanes(event?: LazyLoadEvent) {
    if (this.dataTableHelper.shouldResetPaging(event)) {
      this.paginator.changePage(0);
      return;
    }

    this.dataTableHelper.showLoadingIndicator();

    this._airplanesServiceProxy
      .getAll(
        this.filters.filterText,
        this.dataTableHelper.getSorting(this.dataTable),
        this.dataTableHelper.getSkipCount(this.paginator, event),
        this.dataTableHelper.getMaxResultCount(this.paginator, event),
      )
      .subscribe(result => {
        this.dataTableHelper.totalRecordsCount = result.totalCount;
        this.dataTableHelper.records = result.items;
        this.dataTableHelper.hideLoadingIndicator();
      });
  }

  reloadPage(): void {
    this.paginator.changePage(this.paginator.getPage());
  }

  createAirplane(): void {
    this.createOrEditAirplaneModal.show();
  }

  showHistory(airplane: AirplaneDto): void {
    this.entityTypeHistoryModal.show({
      entityId: airplane.id.toString(),
      entityTypeFullName: this._entityTypeFullName,
      entityTypeDescription: '',
    });
  }

  deleteAirplane(airplane: AirplaneDto): void {
    this.message.confirm('', isConfirmed => {
      if (isConfirmed) {
        this._airplanesServiceProxy.delete(airplane.id).subscribe(() => {
          this.reloadPage();
          this.notify.success(this.l('SuccessfullyDeleted'));
        });
      }
    });
  }

  exportToExcel(): void {
    this._airplanesServiceProxy.getAirplanesToExcel().subscribe(result => {
      this._fileDownloadService.downloadTempFile(result);
    });
  }

  startJob(): void {
    this.dataTableHelper.showLoadingIndicator();
    this._airplanesServiceProxy.startJob().subscribe(() => {
      this.dataTableHelper.hideLoadingIndicator();
    });
  }

  registerEvents(): void {
    eaf.event.on('app.realtime.messageReceived', message => {
      this.notify.success(message);
    });
  }
}
