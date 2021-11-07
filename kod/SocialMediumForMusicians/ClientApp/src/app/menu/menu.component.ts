import { Component, Input } from "@angular/core";

@Component({
    selector: 'app-menu',
    templateUrl: './menu.component.html'
})
export class MenuComponent {
    isVisible = false;

    @Input() isMusician: boolean;

    constructor() {}

    clicked() {
        this.isVisible = !this.isVisible;
    }
}
