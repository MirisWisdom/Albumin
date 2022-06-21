<?php

namespace App\Http\Controllers;

use App\Models\Record;
use App\Models\Source;
use Exception;
use Illuminate\Contracts\Foundation\Application;
use Illuminate\Contracts\View\Factory;
use Illuminate\Contracts\View\View;
use Illuminate\Http\RedirectResponse;
use Illuminate\Http\Request;

class SourceController extends Controller
{
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
    public function show(Source $source): View|Factory|Application
    {
        return view('sources', [
            'source'  => $source,
            'records' => Record::query()
                               ->where('source_id', $source->id)
                               ->has('entries')
                               ->with('entries')->get()
        ]);
    }
}
