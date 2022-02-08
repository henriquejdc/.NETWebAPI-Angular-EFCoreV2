import {  AbstractControl,
          ValidationErrors,
          FormBuilder,
          FormArray,
          FormControl,
          FormGroup,
          Validators
        } from '@angular/forms';
        import { Component, OnInit, TemplateRef } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Evento } from '@app/models/Evento';
import { Lote } from '@app/models/Lote';
import { EventoService } from '@app/services/evento.service';
import { LoteService } from '@app/services/lote.service';

import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { BsLocaleService } from 'ngx-bootstrap/datepicker';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';
import { environment } from '@environments/environment';

@Component({
  selector: 'app-evento-detalhe',
  templateUrl: './evento-detalhe.component.html',
  styleUrls: ['./evento-detalhe.component.scss']
})

export class EventoDetalheComponent implements OnInit {

  modalRef?: BsModalRef;
  eventoId: number;
  evento = {} as Evento;
  form : FormGroup = this.FormBuilder.group({});
  estadoSalvar = 'post';
  loteAtual = {id: 0, nome: '', indice: 0}
  imagemURL = 'assets/cloud.png';
  file: File;

  get modoEditar(): boolean {
    return this.estadoSalvar === 'put';
  }


  get lotes() : FormArray {
    return this.form.get('lotes') as FormArray;
  }

  get f(): any{
    return this.form.controls;
  }

  get bsConfig(): any {
    return {
      isAnimated: true,
      adaptivePosition: true,
      dateInputFormat: 'DD/MM/YYYY hh:mm a',
      containerClass: 'theme-default',
      showWeekNumbers: false
    }
  }

  get bsConfigLote(): any {
    return {
      isAnimated: true,
      adaptivePosition: true,
      dateInputFormat: 'DD/MM/YYYY',
      containerClass: 'theme-default',
      showWeekNumbers: false
    }
  }

  constructor(private FormBuilder: FormBuilder,
              private localeService: BsLocaleService,
              private activatedRouter: ActivatedRoute,
              private eventoService: EventoService,
              private spinner: NgxSpinnerService,
              private toastr: ToastrService,
              private router: Router,
              private loteService: LoteService,
              private modalService: BsModalService)
  {
    this.localeService.use('pt-br');
  }

  public carregarEvento() : void {
    this.eventoId = +this.activatedRouter.snapshot.paramMap.get('id');
    this.estadoSalvar = 'post';

    if (this.eventoId != null && this.eventoId != 0)
    {
      this.estadoSalvar = 'put';
      this.spinner.show();
      this.eventoService.getEventoById(this.eventoId).subscribe({
        next: (evento: Evento) => {
          this.evento = {...evento};
          this.form.patchValue(this.evento);
          if (this.evento.imagemURL != '')
            this.imagemURL = environment.api + 'resources/images/' + this.evento.imagemURL;
          this.carregarLotes();
          // this.evento.lotes.forEach(lote => {
          //   this.lotes.push(this.criarLote(lote))
          // });
        },
        error: (error: any) => {
          this.spinner.hide(),
          this.toastr.error('Erro ao carrgar evento!', 'Erro!'),
          console.error(error)
        },
        complete: () => {this.spinner.hide()},
      });
    }
  }

  public validation(): void{
    this.form = this.FormBuilder.group({
      tema: ['', [
          Validators.required,
          Validators.minLength(4),
          Validators.maxLength(50)]],
      local: ['', Validators.required],
      dataEvento: ['', Validators.required],
      qtdPessoas: ['', [
          Validators.required,
          Validators.max(120000)]],
      telefone: ['', Validators.required],
      email: ['', [
          Validators.required,
          Validators.email
        ]],
      imagemURL: [''],
      lotes: this.FormBuilder.array([])
    });
  }

  adicionarLote(): void {
    this.lotes.push(this.criarLote({id: 0} as Lote));
  }

  criarLote(lote: Lote): FormGroup {
    return this.FormBuilder.group({
      id: [lote.id],
      nome: [lote.nome, Validators.required],
      quantidade: [lote.quantidade, Validators.required],
      preco: [lote.preco, Validators.required],
      dataInicio: [lote.dataInicio],
      dataFim: [lote.dataFim],
    })
  }

  public resetForm(): void {
    this.form.reset();
  }

  public cssValidator(camppoForm: FormControl | AbstractControl): any {
    return {'is-invalid': camppoForm.errors && camppoForm.touched}
  }

  public salvarEvento() : void {
    this.spinner.show();
    if (this.form.valid){

      if (this.estadoSalvar === 'post'){
        this.evento = {... this.form.value}
      } else {
        this.evento = {id: this.evento.id, ... this.form.value}
      }

      this.eventoService[this.estadoSalvar](this.evento).subscribe(
        (eventoRetorno: Evento) => {
          this.toastr.success('Evento salvo com sucesso.', 'Salvo!');
          this.router.navigate([`eventos/detalhe/${eventoRetorno.id}`]);
        },
        (error: any) => {
          console.log(error);
          this.toastr.error('Error ao salvar.', 'Erro!')
        }
      ).add(() => this.spinner.hide());
    }
  }

  public salvarLotes() : void{
    if (this.form.controls['lotes'].valid){
      this.spinner.show();
      this.loteService.save(this.eventoId, this.form.value.lotes)
          .subscribe(
            () => {
              this.toastr.success('Lotes foram salvos com sucesso!', "Sucesso!");
              this.router.navigate([`eventos/detalhe/${this.eventoId}`]);
            },
            (error: any) => {
              this.toastr.error('Erro ao salvar os lotes!', "Erro!");
              console.error(error);
            },
          ).add(() => this.spinner.hide());

    }
  }

  carregarLotes() : void {
    this.spinner.show();
    this.loteService.getLotesByEventoId(this.eventoId).subscribe(
      (lotes: Lote[]) => {
        lotes.forEach(lote => {
          this.lotes.push(this.criarLote(lote))
        });
      },
      (error: any) => {
        this.toastr.error('Erro ao carregar os lotes!', "Erro!");
        console.error(error);
      },
    ).add(() => this.spinner.hide());
  }

  removerLote(template: TemplateRef<any>, indice: number) : void {

    this.loteAtual.id = this.lotes.get(indice + '.id').value;
    this.loteAtual.nome = this.lotes.get(indice + '.nome').value;
    this.loteAtual.indice = indice;

    // this.lotes.removeAt(indice);
    this.modalRef = this.modalService.show(template, {class:'modal-sm'});
  }

  declineDeleteLote(): void {
    this.modalRef?.hide();
  }

  confirmDeleteLote(): void {
    this.modalRef?.hide();
    this.spinner.show();

    this.loteService.deleteLote(this.eventoId, this.loteAtual.id)
        .subscribe(
          () => {
            this.toastr.success('Lote deletado com sucesso!', "Sucesso!");
            this.lotes.removeAt(this.loteAtual.indice);
          },
          (error: any) => {
            this.toastr.error(`Erro ao tentar deletar Lote ${this.loteAtual.id}`, 'Erro!');
            console.error(error);
          },
        ).add(() => this.spinner.hide());
  }

  mudarValorData(value: Date, indice: number, campo: string): void {
    this.lotes.value[indice][campo] = value;
  }

  retornaTituloLote(nome: string): string {
    return nome === null || nome === '' ? 'Nome do Lote' : nome;
  }

  onFileChange(ev: any): void {
    const reader = new FileReader();

    reader.onload = (event: any) => this.imagemURL = event.target.result;

    this.file = ev.target.files;

    reader.readAsDataURL(this.file[0]);

    this.uploadImage();
  }

  uploadImage(): void {
    this.spinner.show();
    this.eventoService.postUpload(this.eventoId, this.file).subscribe(
      () => {
        this.carregarEvento();
        this.toastr.success('Imagem atualizada com sucesso.', 'Sucesso!');
      },
      (error: any) => {
        this.toastr.error(`Erro ao atualizar imagem`, 'Erro!');
        console.error(error);
      },
    ).add(() => this.spinner.hide());
  }


  ngOnInit(): void {
    this.carregarEvento();
    this.validation();
  }

}
