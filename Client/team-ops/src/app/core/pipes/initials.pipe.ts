import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'initials',
  standalone: true,
})
export class InitialsPipe implements PipeTransform {
  transform(value: string): string {
    let words = value.split(' ');
    let retVal = '';
    if (words.length == 1) {
      retVal = words[0].slice(0, 2).toUpperCase();
    } else {
      words.forEach((w) => {
        retVal = retVal.concat(w[0].toUpperCase());
      });
    }
    return retVal;
  }
}
