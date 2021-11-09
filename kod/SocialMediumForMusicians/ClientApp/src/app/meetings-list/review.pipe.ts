import { Pipe, PipeTransform } from "@angular/core";

@Pipe({
    name: 'isReviewButton'
})
export class MyReviewPipe implements PipeTransform {
    transform(value: any, ...args: any[]) {
        const date = new Date(value);
        return date.getDate() < Date.now();
    }
}
