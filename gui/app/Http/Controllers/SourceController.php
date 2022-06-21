<?php

namespace App\Http\Controllers;

use App\Http\Requests\UpdateSourceRequest;
use App\Models\Record;
use App\Models\Source;
use Exception;
use Illuminate\Contracts\Foundation\Application;
use Illuminate\Contracts\View\Factory;
use Illuminate\Contracts\View\View;
use Illuminate\Http\RedirectResponse;
use Illuminate\Http\Request;
use Illuminate\Http\Response;

class SourceController extends Controller
{
    /**
     * Display a listing of the resource.
     *
     * @return Response
     */
    public function index()
    {
        //
    }

    /**
     * Show the form for creating a new resource.
     *
     * @return Response
     */
    public function create()
    {
        //
    }

    /**
     * Store a newly created resource in storage.
     *
     * @param Request $request
     * @return RedirectResponse
     */
    public function store(Request $request): RedirectResponse
    {
        try {
            $source = Source::store(trim($request->input('source')));

            if ($source->records()->has('entries')->count() == 0) {
                $record = Record::store($source);
                return redirect()->route('sources.records.show', [$source, $record]);
            }

            return redirect()->route('sources.show', $source);
        } catch (Exception $exception) {
            return redirect()->route('welcome')->with(['error' => $exception->getMessage()]);
        }
    }

    /**
     * Display the specified resource.
     *
     * @param Source $source
     * @return Application|Factory|View
     */
    public function show(Source $source)
    {
        return view('sources', [
            'source'  => $source,
            'records' => Record::query()
                               ->where('source_id', $source->id)
                               ->has('entries')
                               ->with('entries')->get()
        ]);
    }

    /**
     * Show the form for editing the specified resource.
     *
     * @param Source $source
     * @return Response
     */
    public function edit(Source $source)
    {
        //
    }

    /**
     * Update the specified resource in storage.
     *
     * @param UpdateSourceRequest $request
     * @param Source $source
     * @return Response
     */
    public function update(UpdateSourceRequest $request, Source $source)
    {
        //
    }

    /**
     * Remove the specified resource from storage.
     *
     * @param Source $source
     * @return Response
     */
    public function destroy(Source $source)
    {
        //
    }
}
