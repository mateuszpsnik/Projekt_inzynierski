import { Pipe, PipeTransform } from "@angular/core";

@Pipe({
    name: 'myTime'
})
export class MyTimePipe implements PipeTransform {
    transform(value: any, ...args: any[]) {
        const date = new Date(value);
        return date.toLocaleTimeString('pl-PL', { timeZone: 'Europe/Warsaw' });
    }
}
