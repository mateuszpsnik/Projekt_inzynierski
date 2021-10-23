import { NgModule } from "@angular/core";
import { Routes, RouterModule } from "@angular/router";
import { HomeComponent } from "./home/home.component";
import { MusicianComponent } from "./musician/musician.component";
import { MusiciansComponent } from "./musicians/musicians.component";

const routes: Routes = [
    { path: '', component: HomeComponent, pathMatch: 'full' },
    { path: 'Musicians', component: MusiciansComponent },
    { path: 'Musician/:id', component: MusicianComponent }
];

@NgModule({
    imports: [RouterModule.forRoot(routes)],
    exports: [RouterModule]
})

export class AppRoutingModule {}
