using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using senai.hroads.webApi_.Domains;
using senai.hroads.webApi_.Interfaces;
using senai.hroads.webApi_.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace senai.hroads.webApi_.Controllers
{

    //resposta da API em json
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class ClassesController : ControllerBase
    {
        private IClasseRepository _classeRepository { get; set; }

        public ClassesController()
        {
            _classeRepository = new ClasseRepository();
        }
        
        /// <summary>
        /// Lista todas as classes
        /// </summary>
        /// <returns>Uma lista de classes e um status code.</returns>
        [HttpGet]
        public IActionResult ListarTodos()
        {
            try
            {
                return Ok(_classeRepository.Listar());
            }
            catch (Exception excep)
            {

                return BadRequest(excep);
            }
        }

        /// <summary>
        /// Lista uma classe pelo seu Id
        /// </summary>
        /// <returns>Uma classe e um status code.</returns>
        [HttpGet("{id}")]
        public IActionResult BuscarPorId(int id)
        {
            Classe classeBuscada = _classeRepository.ListarPorId(id);


            if (classeBuscada == null)
            {
                return NotFound(
                    new
                    {
                        mensagem = "Essa classe não existe ou algo deu errado !",
                        erro = true
                    }
                    );
            }

            try
            {
                return Ok(classeBuscada);
            }
            catch (Exception excep)
            {

                return BadRequest(excep);
            }
        }

        /// <summary>
        /// Atualiza uma classe existente pelo seu Id
        /// </summary>
        /// <param name="id">Id da classe</param>
        /// <param name="classeAtualizada">Objeto onde está as informções atualizadas</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public IActionResult AtualizarPorId(int id, Classe classeAtualizada)
        {
            Classe classeBuscada = _classeRepository.ListarPorId(id);


            if (classeBuscada == null)
            {
                return NotFound(
                    new
                    {
                        mensagem = "Essa classe não existe ou algo deu errado !",
                        erro = true
                    }
                    );
            }

            try
            {
                _classeRepository.Atualizar(id, classeAtualizada);
                return NoContent();
            }
            catch (Exception execp)
            {
                return BadRequest(execp);
            }
        }
        
        /// <summary>
        /// Cadastra uma nova classe
        /// </summary>
        /// <returns>Um status code 201 - Created</returns>
        [Authorize(Roles = "1")]
        [HttpPost]
        public IActionResult Cadastrar(Classe novaClasse)
        {

            if (novaClasse != null)
            {
                try
                {
                    _classeRepository.Cadastrar(novaClasse);
                    return Ok();
                }
                catch (Exception execp)
                {
                    return BadRequest(execp);
                }
            }

            return NotFound(
                new
                {
                    mensagem = "Campo vazio !",
                    erro = true
                }


                );

        }
        
        /// <summary>
        /// Deleta uma classe existente
        /// </summary>
        /// <param name="id">id da classe que será deletada</param>
        /// <returns>Um status code 204 - No Content</returns>
        /// ex: http://localhost:5000/api/classe/excluir/3
        [HttpDelete("{id}")]
        public IActionResult Deletar(int id)
        {
            Classe classebuscada = _classeRepository.ListarPorId(id);

            if (classebuscada != null)
            {
                try
                {
                    _classeRepository.Deletar(id);
                    return NoContent();
                }
                catch (Exception execp)
                {
                    return BadRequest(execp);
                }
            }

            return NotFound("Id não encontrado ou o campo está vazio !");
        }
    }
}
