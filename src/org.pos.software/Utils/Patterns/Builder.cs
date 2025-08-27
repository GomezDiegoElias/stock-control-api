namespace org.pos.software.Utils.Patterns
{
    public abstract class Builder<TBuilder, TEntity>
    where TBuilder : Builder<TBuilder, TEntity>
    where TEntity : new()
    {
        protected readonly TEntity _entity = new TEntity();

        public TEntity Build() => _entity;

        // Método helper para retornar el builder actual con el tipo correcto
        protected TBuilder This => (TBuilder)this;

    }

}
