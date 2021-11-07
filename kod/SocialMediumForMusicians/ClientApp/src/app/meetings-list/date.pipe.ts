import { Pipe, PipeTransform } from "@angular/core";

@Pipe({
    name: 'myDate'
})
export class MyDatePipe implements PipeTransform {
    transform(value: any, ...args: any[]) {
        const date = new Date(value);
        return date;
    }
}
