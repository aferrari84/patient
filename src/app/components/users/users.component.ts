import { Component, OnInit } from '@angular/core';

import { User } from '../../models/user';
import { UserService } from '../../services/user.service';

@Component({
  selector: 'app-users',
  templateUrl: './users.component.html',
  styleUrls: ['./users.component.css']
})
export class UsersComponent implements OnInit {
  users: User[];
  user: User;
  userStatus: boolean;

  constructor(private userService: UserService) { }

  ngOnInit() {
    this.getUsers();
    this.user = new User;
    this.user.name = 'Pablo';

    this.userStatus = true;
  }

  
  getUsers(): void {
    this.userService.getUsers()
    .subscribe(users => this.users = users);
  }

  /*

  add(name: string): void {
    name = name.trim();
    if (!name) { return; }
    this.heroService.addHero({ name } as Hero)
      .subscribe(hero => {
        this.heroes.push(hero);
      });
  }

  delete(hero: Hero): void {
    this.heroes = this.heroes.filter(h => h !== hero);
    this.heroService.deleteHero(hero).subscribe();
  }
*/
}
