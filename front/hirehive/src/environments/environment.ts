import { InjectionToken } from "@angular/core";

export const environment = {
    apiBaseUrl: "https://localhost:7206/api"
};
export const API_BASE_URL = new InjectionToken<string>('API_BASE_URL');
