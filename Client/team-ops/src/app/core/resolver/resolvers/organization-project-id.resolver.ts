import { inject } from '@angular/core';
import { ResolveFn } from '@angular/router';
import { DataService } from '../services/data.service';
import { Observable, map, switchMap } from 'rxjs';

export const organizationProjectIdResolver: ResolveFn<Observable<any>> = (
  route,
  state
) => {
  let dataService = inject(DataService);
  const organizationName = route.paramMap.get('organizationName')!;
  const projectName = route.paramMap.get('projectName')!;

  return dataService.resolveOrganizationId(organizationName).pipe(
    switchMap((response: any) => {
      console.log(response)
      const organizationId = response.id;
      return dataService.resolveProjectId(projectName, organizationId).pipe(
        map((secondResponse: any) => {
          console.log(secondResponse)
          const projectId = secondResponse.id;
          return { organizationId, projectId };
        })
      );
    })
  );
};
