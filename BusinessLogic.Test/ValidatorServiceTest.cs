using System.Reflection;
using System.Reflection.Emit;
using BusinessLogic.Validators;
using FluentAssertions;
using ModeloValidador.Abstracciones;

[TestClass]
public class ValidatorServiceTests
{
    private string _tempDirectory;
    private ValidatorService _validatorService;

    [TestInitialize]
    public void Setup()
    {
        _tempDirectory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        Directory.CreateDirectory(_tempDirectory);

        var assembly = CreateMockValidatorAssembly();

        _validatorService = new ValidatorService();

        foreach (var type in assembly.GetTypes())
        {
            if (typeof(IModeloValidador).IsAssignableFrom(type))
            {
                typeof(ValidatorService)
                    .GetField("implementations", BindingFlags.NonPublic | BindingFlags.Instance)
                    ?.SetValue(_validatorService, new List<Type> { type });
            }
        }
    }

    [TestMethod]
    public void ChargeValidators_ShouldLoadValidators()
    {
        var loadedValidators = _validatorService.ChargeValidators();

        loadedValidators.Should().ContainSingle()
            .And.Contain("MockValidator");
    }

    [TestMethod]
    public void GetValidator_ShouldReturnValidatorInstance()
    {
        _validatorService.ChargeValidators();

        var validatorInstance = _validatorService.GetValidator(0);

        validatorInstance.Should().NotBeNull();
        validatorInstance.GetType().Name.Should().Be("MockValidator");
    }

    [TestMethod]
    public void GetValidatorByName_ShouldReturnValidatorInstanceByName()
    {
        _validatorService.ChargeValidators();

        var validatorInstance = _validatorService.GetValidatorByName("MockValidator");

        validatorInstance.Should().NotBeNull();
        validatorInstance.GetType().Name.Should().Be("MockValidator");
    }

    [TestMethod]
    public void GetValidatorByName_InvalidName_ShouldThrowException()
    {
        Action act = () => _validatorService.GetValidatorByName("NombreInexistente");
        act.Should().Throw<ArgumentException>().WithMessage("No validator found by the name: NombreInexistente");
    }

    private Assembly CreateMockValidatorAssembly()
    {
        var assemblyName = new AssemblyName("MockValidatorAssembly");
        var assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run);
        var moduleBuilder = assemblyBuilder.DefineDynamicModule("MainModule");

        var typeBuilder = moduleBuilder.DefineType("MockValidator", TypeAttributes.Public, null, [typeof(IModeloValidador)]);

        typeBuilder.DefineDefaultConstructor(MethodAttributes.Public);

        var esValidoMethodBuilder = typeBuilder.DefineMethod(
            nameof(IModeloValidador.EsValido),
            MethodAttributes.Public | MethodAttributes.Virtual,
            typeof(bool),
            [typeof(Modelo)]
        );

        var ilGenerator = esValidoMethodBuilder.GetILGenerator();
        ilGenerator.Emit(OpCodes.Ldc_I4_1);
        ilGenerator.Emit(OpCodes.Ret);

        typeBuilder.DefineMethodOverride(esValidoMethodBuilder, typeof(IModeloValidador).GetMethod(nameof(IModeloValidador.EsValido)) ?? throw new InvalidOperationException());

        typeBuilder.CreateType();

        return assemblyBuilder;
    }
}
