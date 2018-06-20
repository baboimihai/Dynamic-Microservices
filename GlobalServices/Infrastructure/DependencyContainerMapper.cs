using ClassLibraryMath;
using LibraryMasterOfNumbers;
using MicroCore;
using PdfDiploma;
using SimpleInjector;
using UniverseOfMath;
using WriteLogToDb;

namespace GlobalServices
{
    public class DependencyContainerMapper
    {
        public static void InitializeContainer(Container container, Lifestyle lifeStyle)
        {
            //container.RegisterConditional(typeof(IRepository<>), typeof(Repository<>), lifeStyle, x => !x.Handled);
            container.Register<ILibraryMath, LibraryMath>(lifeStyle);
            MicroContainer.RegisterMicro<LibraryMath>(); //register micro
            container.Register<IMasterOfNumbers, MasterOfNumbers>(lifeStyle);
            MicroContainer.RegisterMicro<MasterOfNumbers>(); //register micro
            container.Register<IUniverseOfSuperMath, UniverseOfSuperMath>(lifeStyle);
            MicroContainer.RegisterMicro<UniverseOfSuperMath>(); //register micro
            container.Register<IClientWriteLog, ClientWriteLog>(lifeStyle);
            MicroContainer.RegisterMicro<ClientWriteLog>(); //register micro
            container.Register<IGenerateDiploma, GenerateDiploma>(lifeStyle);
            MicroContainer.RegisterMicro<GenerateDiploma>(); //register micro

            container.Register<IExamples, Examples>(lifeStyle);

        }

    }
}
