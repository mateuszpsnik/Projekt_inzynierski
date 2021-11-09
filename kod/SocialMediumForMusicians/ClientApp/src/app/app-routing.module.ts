import { NgModule } from "@angular/core";
import { Routes, RouterModule } from "@angular/router";
import { AuthorizeGuard } from "src/api-authorization/authorize.guard";
import { HomeComponent } from "./home/home.component";
import { MeetingsComponent } from "./meetings/meetings.component";
import { MusicianComponent } from "./musician/musician.component";
import { MusiciansComponent } from "./musicians/musicians.component";
import { ReviewComponent } from "./review/review.component";

const routes: Routes = [
    { path: '', component: HomeComponent, pathMatch: 'full' },
    { path: 'Musicians', component: MusiciansComponent },
    { path: 'Musician/:id', component: MusicianComponent },
    { path: 'Meetings', component: MeetingsComponent,
        canActivate: [AuthorizeGuard] },
    { path: 'Reviews/:id', component: ReviewComponent,
        canActivate: [AuthorizeGuard] }
];

@NgModule({
    imports: [RouterModule.forRoot(routes)],
    exports: [RouterModule]
})

export class AppRoutingModule {}
