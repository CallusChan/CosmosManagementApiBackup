using AutoMapper;
using CosmosManagementApi.Dtos;
using CosmosManagementApi.Models;

namespace CosmosManagementApi.Profiles
{
  public class CosmosManagementProfile : Profile
  {
    public CosmosManagementProfile()
    {
      CreateMap<Customer, CustomerGetDto>();
      CreateMap<CustomerGetDto, Customer>();
      CreateMap<CustomerAddDto, Customer>();
      CreateMap<Customer, CustomerAddDto>();
      CreateMap<CustomerUpdateDto, Customer>();
      CreateMap<Customer, CustomerUpdateDto>();

      //Product
      CreateMap<Product, ProductGetDto>();
      CreateMap<ProductGetDto, Product>();
      CreateMap<ProductAddDto, Product>();
      CreateMap<Product, ProductAddDto>();
      CreateMap<ProductClass, ProductClassDto>();
      CreateMap<ProductClassDto, ProductClass>();
      CreateMap<ProductAddDto, ProductClassDto>();
      CreateMap<ProductClassDto, ProductAddDto>();
      CreateMap<ProductAddDto, ProductClass>();
      CreateMap<ProductClass, ProductAddDto>();
      CreateMap<ProductClassAddDto, ProductAddDto>();
      CreateMap<ProductAddDto, ProductClassAddDto>();
      CreateMap<ProductClassUpdateDto, ProductClass>();
      CreateMap<ProductClass, ProductClassUpdateDto>();
      CreateMap<ProductClass, ProductClassAddDto>();
      CreateMap<ProductClassAddDto, ProductClass>();

      CreateMap<Project, ProjectGetDto>();
      CreateMap<ProjectGetDto, Project>();
      CreateMap<ProjectAddDto, Project>();
      CreateMap<Project, ProjectAddDto>();
      CreateMap<Project, ProjectUpdateDto>();
      CreateMap<ProjectUpdateDto, Project>();

      //ProjectCategory
      CreateMap<ProjectCategoriesGetDto, ProjectCategory>();
      CreateMap<ProjectCategoriesPostDto, ProjectCategory>();
      CreateMap<ProjectCategory, ProjectCategoriesGetDto>();
      CreateMap<ProjectCategory, ProjectCategoriesPostDto>();

      //PurchaseCategory
      CreateMap<PurchaseCategoriesGetDto, PurchaseCategory>();
      CreateMap<PurchaseCategoriesPostDto, PurchaseCategory>();
      CreateMap<PurchaseCategory, PurchaseCategoriesGetDto>();
      CreateMap<PurchaseCategory, PurchaseCategoriesPostDto>();


      CreateMap<StaffAddDto, Staff>();
      CreateMap<Staff, StaffAddDto>();
      CreateMap<StaffGetDto, Staff>();
      CreateMap<Staff, StaffGetDto>();
      CreateMap<Staff, StaffUpdateDto>();
      CreateMap<StaffUpdateDto, Staff>();


      //Staff login mapper
      CreateMap<StaffAccount, StaffLoginDto>();
      CreateMap<StaffLoginDto, StaffAccount>();
      CreateMap<StaffAccount, StaffAccountCreateDto>();
      CreateMap<StaffAccountCreateDto, StaffAccount>();

      //Bill map
      CreateMap<Bill, BillDto>();
      CreateMap<BillDto, Bill>();

    }
  }
}
