import { EafHttpInterceptor } from '@eaf/eafHttpInterceptor';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { NgModule } from '@angular/core';
import * as ApiServiceProxies from './service-proxies';

@NgModule({
    providers: [
        ApiServiceProxies.AirplanesServiceProxy,
        ApiServiceProxies.CachingServiceProxy,
        ApiServiceProxies.WebLogServiceProxy,
        ApiServiceProxies.AuditLogServiceProxy,
        ApiServiceProxies.CommonLookupServiceProxy,
        ApiServiceProxies.HostSettingsServiceProxy,
        ApiServiceProxies.LanguageServiceProxy,
        ApiServiceProxies.NotificationServiceProxy,
        ApiServiceProxies.PermissionServiceProxy,
        ApiServiceProxies.ProfileServiceProxy,
        ApiServiceProxies.RoleServiceProxy,
        ApiServiceProxies.SessionServiceProxy,
        ApiServiceProxies.TenantServiceProxy,
        ApiServiceProxies.TimingServiceProxy,
        ApiServiceProxies.UserServiceProxy,
        ApiServiceProxies.UserLoginServiceProxy,
        ApiServiceProxies.AccountServiceProxy,
        ApiServiceProxies.TokenAuthServiceProxy,
        ApiServiceProxies.UiCustomizationSettingsServiceProxy,
        ApiServiceProxies.DynamicSettingsServiceProxy,
        ApiServiceProxies.FileServiceProxy,
        ApiServiceProxies.FriendshipServiceProxy,
        ApiServiceProxies.ChatServiceProxy,
        ApiServiceProxies.AboutServiceProxy,
        ApiServiceProxies.WebhookSubscriptionServiceProxy,
        ApiServiceProxies.AirportsServiceProxy,
        ApiServiceProxies.AwbServiceProxy,
        ApiServiceProxies.TrackingsServiceProxy,
        ApiServiceProxies.ViaCepServiceProxy,
        { provide: HTTP_INTERCEPTORS, useClass: EafHttpInterceptor, multi: true }
    ]
})
export class ServiceProxyModule { }
