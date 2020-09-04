using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using O2.Catalog.API.Domain;

namespace O2.Catalog.API.Data
{
    public class CatalogSeed
    {
        public static async Task SeedAsync(CatalogContext context)
        {
            context.Database.EnsureCreated();
                        
            if (!context.CatalogBrands.Any())
            {
                context.CatalogBrands.AddRange(GetPreconfiguredCatalogBrands());
                await context.SaveChangesAsync();
            }

            if (!context.CatalogTypes.Any())
            {
                context.CatalogTypes.AddRange(GetPreconfiguredCatalogTypes());
                await context.SaveChangesAsync();
            }
            if (!context.CatalogItems.Any())
            {
                context.CatalogItems.AddRange(GetPreconfiguredCatalogItems());
                await context.SaveChangesAsync();
            }
        }

        static IEnumerable<CatalogBrand> GetPreconfiguredCatalogBrands()
        {

            return new List<CatalogBrand>()
            {

                new CatalogBrand()
                {
                    Brand="Антон Марков"
                }
            };
        }

        static IEnumerable<CatalogType> GetPreconfiguredCatalogTypes()
        {
            return new List<CatalogType>()
            {
                new CatalogType{Type="Тренинги"},
                new CatalogType{Type="Видеокурсы"},
                new CatalogType{Type="Мероприятия"},
                new CatalogType{Type="Лечебные сеансы"}
            };
        }

        static IEnumerable<CatalogItem> GetPreconfiguredCatalogItems()
        {
            return new List<CatalogItem>()
            {
                //Traning
                new CatalogItem
                {
                    CatalogBrandId=1,
                    CatalogTypeId=1,
                    Name="Групповой ПФР",
                    Description="Вы обучаетесь в группе от 5 до 10 человек",
                    Price=7900,
                    PictureUrl="http://externalcatalogbaseurltobereplaced/api/pic/1"
                }
                //new CatalogItem
                //{
                //    CatalogBrandId=1,
                //    CatalogTypeId=2,
                //    Name="Обучающий видеокурс: Самый современный курс гипноза с кейсом по обезболиванию 2019",
                //    Description="В данном видео курсе предоставлены самые современные техники и фундаментальные знание о гипнозе, во всех его проявлениях \r\n"+ 
                //    "Готовые кейсты по:  \r\n"+ 
                //    "– Чувство обратной связи, установка и удержание раппорта \r\n"+ 
                //    "– Вербальный и невербальный гипноз  \r\n"+ 
                //    "– Гипнотические тесты, классические и в разговоре (скрытные) с демонстрацией \r\n"+ 
                //    "– Гипнотические индукции (фундамент построение индукйии и различные варианты)с демонстрацией  \r\n",
                //    Price=25000
                //},
                //new CatalogItem
                //{
                //    CatalogBrandId=1,
                //    CatalogTypeId=2,
                //    Name="Обучающий видеокурс: “Обучение гипнозу От и До",
                //    Description="Description 2",
                //    Price=6999
                //},
                //new CatalogItem
                //{
                //    CatalogBrandId=1,
                //    CatalogTypeId=2,
                //    Description="Обучающий видеокурс: Мгновенный гипноз 2.0 & Реальная гипнотерапия", Price=17000
                //},
                //new CatalogItem
                //{
                //    CatalogBrandId=1,
                //    CatalogTypeId=2,
                //    Description="Обучающий видеокурс: Гипнотерапия", Price=19999
                //}
            };
        }
    }
}
