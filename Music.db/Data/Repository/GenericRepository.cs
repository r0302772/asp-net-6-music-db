﻿namespace Music.db.Data.Repository
{
	public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
	{
		private readonly MusicdbContext _context;

		public GenericRepository(MusicdbContext context)
		{
			_context = context;
		}

		public IQueryable<TEntity> GetAll() { return _context.Set<TEntity>(); }
		public async Task<TEntity> GetById(int? id) { return await _context.Set<TEntity>().FindAsync(id) ; }
		public void Create(TEntity entity) { _context.Set<TEntity>().Add(entity); }
		public void Update(TEntity entity) { _context.Set<TEntity>().Update(entity); }
		public void Delete(TEntity entity) { _context.Set<TEntity>().Remove(entity); }
	}
}
