using BusinessLogic.Validators;
using FluentAssertions;
using ModeloValidador.Abstracciones;
using System.Reflection;
using System.Reflection.Emit;

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
        
        loadedValidators.Should().ContainSingle("se esperaba que se cargara exactamente un validador simulado")
            .And.Contain("MockValidator", "el tipo simulado debería llamarse MockValidator");
    }

    [TestMethod]
    public void GetValidator_ShouldReturnValidatorInstance()
    {
        _validatorService.ChargeValidators();

        var validatorInstance = _validatorService.GetValidator(0);

        validatorInstance.Should().NotBeNull("se esperaba una instancia de IModeloValidador");
        validatorInstance.GetType().Name.Should().Be("MockValidator", "la instancia debe ser del tipo MockValidator");
    }

    [TestMethod]
    public void GetValidatorByName_ShouldReturnValidatorInstanceByName()
    {
        _validatorService.ChargeValidators();

        var validatorInstance = _validatorService.GetValidatorByName("MockValidator");

        validatorInstance.Should().NotBeNull("se esperaba una instancia de IModeloValidador");
        validatorInstance.GetType().Name.Should().Be("MockValidator", "la instancia debe ser del tipo MockValidator");
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

        var typeBuilder = moduleBuilder.DefineType("MockValidator", TypeAttributes.Public, null, new[] { typeof(IModeloValidador) });

        typeBuilder.DefineDefaultConstructor(MethodAttributes.Public);

        typeBuilder.CreateType();

        return assemblyBuilder;
    }
}
