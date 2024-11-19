import { Component } from '@angular/core';
import { UserService } from '../../../backend/services/users.service';
import { DynamicTableComponent } from '../dynamic-table/dynamic-table.component';
import { Router } from '@angular/router';

@Component({
  selector: 'app-users-content',
  standalone: true,
  imports: [DynamicTableComponent],
  templateUrl: './users-content.component.html',
  styleUrls: ['./users-content.component.css']
})
export class UsersContentComponent {
  users: any[] = [DynamicTableComponent];
  rows: { [key: string]: string }[] = [];
  columns = ['Name', 'LastName', 'Role', 'CreatedAt', 'Actions'];

  constructor(private usersService: UserService, private router: Router) {}

  ngOnInit() {
    this.usersService.getUsers().subscribe(response => {
      this.users = response.users;
      this.rows = this.users.map(user => ({
        Id: user.id,
        Name: user.name.toString(),
        LastName: user.lastName.toString(),
        Role: user.role.name.toString(),
        CreatedAt: user.createdAt.toString(),
        Actions: 'delete' // Indica que esta fila tendrá acciones (botón)
      }));
    });
  }

  onRowClick(row: any): void {
    // Aquí verificamos si el clic es sobre el botón de acciones (p.ej., eliminar)
    if (row.Actions !== 'delete') {
      this.router.navigate(['users', row.Id]);
    }
  }

  deleteUser(userId: string): void {
    if (confirm('¿Estás seguro de que deseas eliminar este usuario?')) {
      this.usersService.deleteUser(userId).subscribe(
        () => {
          this.rows = this.rows.filter(user => user['Id'] !== userId);
          alert('Usuario eliminado con éxito');
        },
        error => {
          console.error('Error eliminando usuario:', error);
          alert('Ocurrió un error al intentar eliminar el usuario');
        }
      );
    }
  }
}

