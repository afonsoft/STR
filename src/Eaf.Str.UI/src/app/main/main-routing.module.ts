import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';

import { AirplanesComponent } from './airplanes/airplanes.component';
import { AirportsComponent } from './airports/airports.component';
import { AwbComponent } from './awb/awb.component';
import { CepComponent } from './cep/cep.component';
import { DashboardComponent } from './dashboard/dashboard.component';
import { TrackingComponent } from './tracking/tracking.component';

@NgModule({
  imports: [
    RouterModule.forChild([
      {
        path: '',
        children: [
          { path: 'airplanes', component: AirplanesComponent, data: { permission: 'Pages.Airplanes' } },
          { path: 'dashboard', component: DashboardComponent, data: { permission: 'Pages.Dashboard' } },
          { path: 'awb', component: AwbComponent, data: { permission: 'Pages.Awb' } },
          { path: 'cep', component: CepComponent, data: { permission: 'Pages.Tracking' } },
          { path: 'tracking', component: TrackingComponent, data: { permission: 'Pages.Tracking' } },
          { path: 'airports', component: AirportsComponent, data: { permission: 'Pages.Airports' } },
        ],
      },
    ]),
  ],
  exports: [RouterModule],
})
export class MainRoutingModule {}
