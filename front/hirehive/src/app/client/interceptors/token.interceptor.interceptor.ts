import { HttpInterceptorFn } from '@angular/common/http';

export const TokenInterceptor: HttpInterceptorFn = (req, next) => {
  return next(req);
};
