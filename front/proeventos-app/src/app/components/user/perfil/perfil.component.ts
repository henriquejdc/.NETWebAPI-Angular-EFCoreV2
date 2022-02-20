import { Component, OnInit } from '@angular/core';
import { AbstractControl, AbstractControlOptions, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ValidatorField } from '@app/helpers/ValidatorField';
import { AccountService } from '@app/services/account.service';
import { UserUpdate } from '@app/models/identity/UserUpdate';
import { NgxSpinner, NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-perfil',
  templateUrl: './perfil.component.html',
  styleUrls: ['./perfil.component.scss']
})

export class PerfilComponent implements OnInit {

  userUpdate = {} as UserUpdate;
  form : FormGroup = this.FormBuilder.group({});

  constructor(
              public FormBuilder: FormBuilder,
              public accountService: AccountService,
              private router: Router,
              private toastr: ToastrService,
              private spinner: NgxSpinnerService) { }

  get f(): any{
    return this.form.controls;
  }

  ngOnInit(): void {
    this.validation();
    this.carregarUsuario();
  }

  private carregarUsuario(): void {
    this.spinner.show();
    this.accountService.getUser().subscribe(
      (userRetorno: UserUpdate) =>
      {
        this.userUpdate = userRetorno;
        this.form.patchValue(this.userUpdate);
        this.toastr.success('Usuário Carregado', 'Sucesso');
      },
      (error) =>
      {
        this.toastr.error(error, 'Erro');
        this.router.navigate(['/dashboard']);
      },
    ).add(() => this.spinner.hide());
  }

  private validation(): void {

    const formOptions: AbstractControlOptions = {
      validators: ValidatorField.MustMatch('password', 'confirmePassword')
    }

    this.form = this.FormBuilder.group({
      userName: [''],
      titulo:['NaoInformado', [Validators.required]],
      primeiroNome:['', [Validators.required]],
      ultimoNome:['', [Validators.required]],
      email:['', [Validators.required, Validators.email]],
      phoneNumber:['', [Validators.required]],
      funcao:['NaoInformado', [Validators.required]],
      descricao:['', [Validators.required, Validators.maxLength(100)]],
      password:['', [Validators.required, Validators.minLength(6)]],
      confirmePassword:['', [Validators.required, Validators.minLength(6)]],
    }, formOptions)
  }

  public resetForm(event: any): void {
    event.preventDefault();
    this.form.reset();
  }

  public onSubmit(): void {
    if (this.form.invalid){
      this.atualizarUsuario();
    }
  }

  public atualizarUsuario() {
    this.userUpdate = { ... this.form.value }
    this.spinner.show();
    this.accountService.updateUser(this.userUpdate).subscribe(
      () => { this.toastr.success('Usuário atualizado!', 'Sucesso') },
      (error) => {
        console.log(error);
        this.toastr.error(error.error, 'Erro')
      }
    ).add(() => this.spinner.hide());
  }

}
