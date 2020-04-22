﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.Models;

namespace WebStore.Infrastructure.Interfaces
{
    /// <summary>
    /// Интерфейс для работы с сотрудниками
    /// </summary>
   public interface IEmployeesService
   {
       /// <summary>
       /// Получение списка сотрудников
       /// </summary>
       /// <returns></returns>
       IEnumerable<EmployeeView> GetAll();

       /// <summary>
       /// Получение списка сотрудника по id
       /// </summary>
       /// <returns></returns>
       EmployeeView GetById(int id);

       /// <summary>
       /// Сохранить изменения
       /// </summary>
       void Commit();

       /// <summary>
       /// Добавить нового
       /// </summary>
       void AddNew(EmployeeView model);

       /// <summary>
       /// Удалить сотрудника
       /// </summary>
       /// <param name="id"></param>
       void Delete(int id);
   }
}