import { Pipe, PipeTransform } from "@angular/core";

@Pipe({
    name: 'isReviewButton'
})
export class MyReviewPipe implements PipeTransform {
    transform(value: any, ...args: any[]) {
        const dateParsed = new Date(value);
        const dateUtc = new Date(Date.UTC(dateParsed.getFullYear(),
            dateParsed.getMonth(), dateParsed.getDate(), dateParsed.getHours(),
            dateParsed.getMinutes()));
        return dateUtc.getTime() < Date.now();
    }
}
