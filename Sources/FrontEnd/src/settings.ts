export class Settings {

    // when debugging, set this field to the address of backend service. e.g: SERVICE_URL = "http://172.22.143.228/api/schedule"
    // when empty, it is considered as the current location that the page is opened into
    private static SERVICE_URL = "";

    public static GET_SERVICE_URL() {
        if (this.SERVICE_URL)
            return this.SERVICE_URL;
        else {
            let port = "";
            if (window.location.port)
                port = `:${window.location.port}`;
            return `${window.location.protocol}//${window.location.hostname}${port}/api/schedule`;
        }
    }

}