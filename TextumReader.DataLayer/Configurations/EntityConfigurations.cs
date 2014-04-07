using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextumReader.ProblemDomain;

namespace TextumReader.DataLayer.Configurations
{
    public class SharedMaterialConfiguration : EntityTypeConfiguration<SharedMaterial>
    {
        public SharedMaterialConfiguration()
        {
            Map(_ => _.ToTable("SharedMaterials"));
            HasKey(_ => _.SharedMaterialId);
        }
    }

    public class WordFrequencyConfiguration : EntityTypeConfiguration<WordFrequency>
    {
        public WordFrequencyConfiguration()
        {
            Map(_ => _.ToTable("WordFrequency"));
            HasKey(_ => _.Id);
        }
    }

    public class AnkiUserConfiguration : EntityTypeConfiguration<AnkiUser>
    {
        public AnkiUserConfiguration()
        {
            Map(_ => _.ToTable("AnkiUsers"));
            HasKey(_ => _.AnkiUserId);
            Property(_ => _.UserId).HasColumnName("UserId");
        }
    }


    public class WordConfiguration : EntityTypeConfiguration<Word>
    {
        public WordConfiguration()
        {
            Map(_ => _.ToTable("Words"));
            HasKey(_ => _.WordId);
            Property(_ => _.DictionaryId).HasColumnName("DictionaryId");
            HasRequired(_ => _.Dictionary).WithMany(_ => _.Words).HasForeignKey(_ => _.DictionaryId).WillCascadeOnDelete(true);
        }
    }

    public class DictionaryConfiguration : EntityTypeConfiguration<Dictionary>
    {
        public DictionaryConfiguration()
        {
            Map(_ => _.ToTable("Dictionaries"));
            HasKey(_ => _.DictionaryId);
        }
    }

    public class TranslationConfiguration : EntityTypeConfiguration<Translation>
    {
        public TranslationConfiguration()
        {
            Map(_ => _.ToTable("Translations"));
            HasKey(_ => _.TranslationId);
            HasRequired(_ => _.Word).WithMany(_ => _.Translations).HasForeignKey(_ => _.WordId);
        }
    }

    public class MaterialConfiguration : EntityTypeConfiguration<Material>
    {
        public MaterialConfiguration()
        {
            Map(_ => _.ToTable("Materials"));
            HasKey(_ => _.MaterialId);


            Property(_ => _.DictionaryId).HasColumnName("DictionaryId");
            Property(_ => _.UserId).HasColumnName("UserId");

            HasRequired(_ => _.Category).WithMany(_ => _.Materials).HasForeignKey(_ => _.CategoryId);
            HasRequired(_ => _.Dictionary).WithMany(_ => _.Materials).HasForeignKey(_ => _.DictionaryId);
        }
    }

    public class CategoryConfiguration : EntityTypeConfiguration<Category>
    {
        public CategoryConfiguration()
        {
            Map(_ => _.ToTable("Categories"));
            HasKey(_ => _.CategoryId);
        }
    }
}
