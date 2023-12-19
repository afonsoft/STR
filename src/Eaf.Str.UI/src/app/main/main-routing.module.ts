import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';

import { AirplanesComponent } from './airplanes/airplanes.component';
import { AwbComponent } from './awb/awb.component';
import { DashboardComponent } from './dashboard/dashboard.component';

@NgModule({
  imports: [
    RouterModule.forChild([
      {
        path: '',
        children: [
          { path: 'airplanes', component: AirplanesComponent, data: { permission: 'Pages.Airplanes' } },
          { path: 'dashboard', component: DashboardComponent, data: { permission: 'Pages.Dashboard' } },
          { path: 'awb', component: AwbComponent, data: { permission: 'Pages.Awb' } },
        ],
      },
    ]),
  ],
  exports: [RouterModule],
})
export class MainRoutingModule {}
