import { Type } from "@angular/core";

export interface DialogData {
  title?: string;
  component: Type<any>;
  data?: any;
}