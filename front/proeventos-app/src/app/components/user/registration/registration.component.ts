import { Component, OnInit } from '@angular/core';
import { AbstractControl, AbstractControlOptions, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ValidatorField } from '@app/helpers/ValidatorField';

@Component({
  selector: 'app-registration',
  templateUrl: './registration.component.html',
  styleUrls: ['./registration.component.scss']
})
export class RegistrationComponent implements OnInit {

  form : FormGroup = this.FormBuilder.group({});

  constructor(public FormBuilder: FormBuilder) { }

  get f(): any{
    return this.form.controls;
  }

  ngOnInit(): void {
    this.validation();
  }

  private validation(): void {

    const formOptions: AbstractControlOptions = {
      validators: ValidatorField.MustMatch('senha', 'confirmeSenha')
    }

    this.form = this.FormBuilder.group({
      primeiroNome:['', [Validators.required]],
      ultimoNome:['', [Validators.required]],
      email:['', [Validators.required, Validators.email]],
      userName:['', [Validators.required]],
      senha:['', [Validators.required, Validators.minLength(8)]],
      confirmeSenha:['', [Validators.required, Validators.minLength(8)]],
    }, formOptions)
  }

}
