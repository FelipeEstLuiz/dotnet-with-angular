import {
  Component,
  ElementRef,
  inject,
  OnInit,
  signal,
  ViewChild,
} from '@angular/core';
import { AdminService } from '../../../core/services/admin.service';
import { User } from '../../../types/user';

@Component({
  selector: 'app-user-management',
  imports: [],
  templateUrl: './user-management.component.html',
  styleUrl: './user-management.component.css',
})
export class UserManagementComponent implements OnInit {
  @ViewChild('rolesModal') rolesModal!: ElementRef<HTMLDialogElement>;
  private adminService = inject(AdminService);
  protected users = signal<User[]>([]);
  protected availableRoles = ['User', 'Moderator', 'Admin'];
  protected selectedUser: User | null = null;

  async ngOnInit() {
    await this.getUserWithRoles();
  }

  async getUserWithRoles() {
    const users = await this.adminService.getUserWithRoles();
    if (users) this.users.set(users);
  }

  openRolesModal(user: User) {
    this.selectedUser = user;
    this.rolesModal.nativeElement.showModal();
  }

  toggleRole(event: Event, role: string) {
    if (!this.selectedUser) return;

    const isChecked = (event.target as HTMLInputElement).checked;

    if (isChecked) this.selectedUser.roles.push(role);
    else
      this.selectedUser.roles = this.selectedUser.roles.filter(
        (r) => r !== role
      );
  }

  async updateRoles() {
    if (!this.selectedUser) return;

    const updateRoles = await this.adminService.updateUserRoles(
      this.selectedUser.id,
      this.selectedUser.roles
    );

    this.users.update((users) =>
      users.map((u) => {
        if (u.id === this.selectedUser?.id) u.roles = updateRoles;
        return u;
      })
    );

    this.rolesModal.nativeElement.close();
  }
}
