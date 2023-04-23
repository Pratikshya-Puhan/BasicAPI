using BasicAPI.Models;
using System.Data;
using System.Data.SqlClient;

namespace BasicAPI.Repository
{
    public interface ICountryRepository
    {
        Task<List<Country>> GetCountries();
        Task<SingleCountry> GetCountry(int Id);
        Task<Country> Save(Country newCountry);
    }

    public class CountryRepository : ICountryRepository
    {
        private readonly IConfiguration _configuration;

        public CountryRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<List<Country>> GetCountries()
        {
            List<Country> countries = new List<Country>();

            using (var connection = new SqlConnection(_configuration.GetConnectionString("Default")))
            {
                connection.Open();

                SqlCommand cmd = new SqlCommand("SELECT * FROM tblCountry", connection)
                {
                    CommandType = CommandType.Text
                };

                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        countries.Add(new Country()
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            Name = Convert.ToString(reader["Name"]),
                            ShortCode = Convert.ToString(reader["ShortCode"]),
                            ISDCode = Convert.ToString(reader["ISDCode"]),
                            FlagUrl = Convert.ToString(reader["FlagUrl"]),
                        });
                    }
                }
            }

            return countries;
        }

        public async Task<SingleCountry> GetCountry(int Id)
        {
            SingleCountry country = new SingleCountry();

            using (var connection = new SqlConnection(_configuration.GetConnectionString("Default")))
            {
                connection.Open();

                SqlCommand cmd = new SqlCommand("SpGetCountry", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.Add(new SqlParameter("@Id", Id));

                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        country = new SingleCountry()
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            Name = Convert.ToString(reader["Name"]),
                            ShortCode = Convert.ToString(reader["ShortCode"]),
                            ISDCode = Convert.ToString(reader["ISDCode"]),
                            FlagUrl = Convert.ToString(reader["FlagUrl"]),
                        };
                    }

                    while(await reader.NextResultAsync())
                    {
                        List<State> states = new List<State>();

                        while (await reader.ReadAsync())
                        {
                            states.Add(new State()
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                Name = Convert.ToString(reader["Name"]),
                                CountryId = Convert.ToInt32(reader["CountryId"])
                            });
                        }

                        country.States = states;
                    }
                }
            }

            return country;
        }

        public async Task<Country> Save(Country newCountry)
        {
            Country country = new Country();

            using (var connection = new SqlConnection(_configuration.GetConnectionString("Default")))
            {
                connection.Open();

                SqlCommand cmd = new SqlCommand("SpSaveCountry", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.Add(new SqlParameter("@Name", newCountry.Name));
                cmd.Parameters.Add(new SqlParameter("@ShortCode", newCountry.ShortCode));
                cmd.Parameters.Add(new SqlParameter("@ISDCode", newCountry.ISDCode));
                cmd.Parameters.Add(new SqlParameter("@FlagUrl", newCountry.FlagUrl));

                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        country =  new Country()
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            Name = Convert.ToString(reader["Name"]),
                            ShortCode = Convert.ToString(reader["ShortCode"]),
                            ISDCode = Convert.ToString(reader["ISDCode"]),
                            FlagUrl = Convert.ToString(reader["FlagUrl"]),
                        };
                    }
                }
            }

            return country;
        }
    }
}
