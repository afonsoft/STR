﻿import { BsDatepickerConfig, BsDaterangepickerConfig, BsLocaleService } from 'ngx-bootstrap/datepicker';
import { NgxBootstrapLocaleMappingService } from 'assets/lib/ngx-bootstrap/ngx-bootstrap-locale-mapping.service';
import { defineLocale } from 'ngx-bootstrap/chronos';
import { ThemeHelper } from '@app/shared/layout/themes/ThemeHelper';

export class NgxBootstrapDatePickerConfigService {

    static getDaterangepickerConfig(): BsDaterangepickerConfig {
        return Object.assign(new BsDaterangepickerConfig(), {
            containerClass: 'theme-' + NgxBootstrapDatePickerConfigService.getThemeColor()
        });
    }

    static getDatepickerConfig(): BsDatepickerConfig {
        return Object.assign(new BsDatepickerConfig(), {
            containerClass: 'theme-' + NgxBootstrapDatePickerConfigService.getThemeColor()
        });
    }

    static getThemeColor(): string {
        return ThemeHelper.getThemeColor();
    }

    static getDatepickerLocale(): BsLocaleService {
        let localeService = new BsLocaleService();
        localeService.use(eaf.localization.currentLanguage.name);
        return localeService;
    }

    static registerNgxBootstrapDatePickerLocales(): Promise<boolean> {
        if (eaf.localization.currentLanguage.name === 'en') {
            return Promise.resolve(true);
        }

        let supportedLocale = new NgxBootstrapLocaleMappingService().map(eaf.localization.currentLanguage.name).toLowerCase();
        let moduleLocaleName = new NgxBootstrapLocaleMappingService().getModuleName(eaf.localization.currentLanguage.name);

        return new Promise<boolean>((resolve, reject) => {
            import(`ngx-bootstrap/chronos/esm5/i18n/${supportedLocale}.js`)
                .then(module => {
                    defineLocale(eaf.localization.currentLanguage.name.toLowerCase(), module[`${moduleLocaleName}Locale`]);
                    resolve(true);
                }, reject);
        });
    }
}
