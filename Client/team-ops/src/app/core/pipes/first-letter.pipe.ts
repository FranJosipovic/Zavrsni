import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  standalone:true,
  name: 'firstLetter',
})
export class GetFirstLetter implements PipeTransform {
  transform(value: string): string {
    if (!value) {
      return '';
    }
    return value.split(' ')[0][0];
  }
}
