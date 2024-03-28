import { inject } from '@angular/core';
import { ResolveFn } from '@angular/router';
import { Observable } from 'rxjs';
import { DataService } from '../services/data.service';

export const organizationIdResolver: ResolveFn<Observable<any>> = (route, state) => {
  const organizationName = route.paramMap.get('organizationName')!;
  let service = inject(DataService)
  return service.resolveOrganizationId(organizationName)
};
