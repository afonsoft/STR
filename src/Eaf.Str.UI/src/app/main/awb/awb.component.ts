import { Component, Injector, OnInit, ViewChild, ViewEncapsulation } from '@angular/core';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { AppComponentBase } from '@shared/common/app-component-base';
import { AwbDto, AwbServiceProxy } from '@shared/service-proxies/service-proxies';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { Paginator } from 'primeng/paginator';
import { LazyLoadEvent } from 'primeng/primeng';
import { Table } from 'primeng/table';

@Component({
  selector: 'app-awb',
  templateUrl: './awb.component.html',
  styleUrls: ['./awb.component.css'],
  encapsulation: ViewEncapsulation.None,
  animations: [appModuleAnimation()],
})
export class AwbComponent extends AppComponentBase implements OnInit {
  @ViewChild('dataTable', { static: true }) dataTable: Table;
  @ViewChild('paginator', { static: true }) paginator: Paginator;

  filters: {
    filterText: string;
  } = <any>{};

  constructor(
    injector: Injector,
    private _awbServiceProxy: AwbServiceProxy,
    private _fileDownloadService: FileDownloadService,
  ) {
    super(injector);
  }

  ngOnInit(): void {}

  getAwbs(event?: LazyLoadEvent) {
    if (this.dataTableHelper.shouldResetPaging(event)) {
      this.paginator.changePage(0);
      return;
    }

    this.dataTableHelper.showLoadingIndicator();

    this._awbServiceProxy
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

  createAwb(): void {
    //this.createOrEditAirplaneModal.show();
  }

  deleteAwb(awb: AwbDto): void {
    this.message.confirm('', isConfirmed => {
      if (isConfirmed) {
        this._awbServiceProxy.delete(awb.id).subscribe(() => {
          this.reloadPage();
          this.notify.success(this.l('SuccessfullyDeleted'));
        });
      }
    });
  }

  exportToExcel(): void {
    this._awbServiceProxy.getAwbToExcel(this.filters.filterText).subscribe(result => {
      this._fileDownloadService.downloadTempFile(result);
    });
  }
}
