﻿declare namespace eaf {
  namespace signalr {
    let autoConnect: boolean;

    let qs: string;

    let remoteServiceBaseUrl: string;

    let url: string;

    let withUrlOptions: object;

    function connect(): any;

    function startConnection(url: string, configureConnection: Function): Promise<any>;

    namespace hubs {
      let common: any;
    }
  }
}
