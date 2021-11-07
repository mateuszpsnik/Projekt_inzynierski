import { Component, OnInit } from '@angular/core';
import { UserService } from '../user/user.service';
import { User } from 'src/models/User';
import { AuthorizeService } from 'src/api-authorization/authorize.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
})
export class HomeComponent implements OnInit {
  public isAuthenticated: boolean;
  userId: string;

  // logged in user
  public user: User;

  constructor(
    private userService: UserService,
    private authorizeService: AuthorizeService) {}

  ngOnInit() {
    this.authorizeService.isAuthenticated().subscribe(isAuth => {
      this.isAuthenticated = isAuth;
    });

    if (this.isAuthenticated) {
      this.authorizeService.getUser().subscribe(authUser => {
        this.userId = authUser.sub;
      });
      this.userService.get(this.userId).subscribe(user => {
        this.user = user;
      });
    }
  }

  isMusician() {
    if (this.user) {
      return this.user.isMusician;
    }
    return false;
  }
}
