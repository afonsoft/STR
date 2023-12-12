module.exports = {
  packages: {
    'angular2-text-mask': {
      ignorableDeepImportMatchers: [
        /text-mask-core\//,
      ]
    },
    'ngx-swiper-wrapper': {
      ignorableDeepImportMatchers: [
        /text-mask-core\//,
        /swiper-events\//,
      ]
    },
    'ng-brazil': {
      ignorableDeepImportMatchers: [
        /text-mask-core\//,
      ]
    },
  },
};