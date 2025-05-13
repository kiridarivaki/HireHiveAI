import { Injectable } from "@angular/core";
import { environment } from "src/environments/environment";

@Injectable({ providedIn: 'root' })
export class UrlService {
  private readonly base = environment.apiBaseUrl;

  urlFor(controller: string, action?: string, pathParams?: Record<string, string | number>, queryParams?: Record<string, string | number>) {
    let fullPath = `${this.base}/${controller}/`;
    
    if (action) {
      fullPath += `/${action}`;
    }

    if (pathParams) {
      for (const key in pathParams) {
        fullPath = fullPath.replace(`{${key}}`, encodeURIComponent(String(pathParams[key])));
      }
    }

    if (queryParams) {
      const queryString = Object.entries(queryParams)
        .map(([k, v]) => `${encodeURIComponent(k)}=${encodeURIComponent(String(v))}`)
        .join('&');
      fullPath += `?${queryString}`;
    }

    return fullPath;
  }
}
